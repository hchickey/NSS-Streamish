using System.Collections.Generic;
using Streamish.Models;
using Streamish.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Linq;
using System;


namespace Streamish.Repositories
{
    public class UserProfileRepository : BaseRepository, IUserProfileRepository
    {
        public UserProfileRepository(IConfiguration configuration) : base(configuration) { }

        public List<UserProfile> GetAllUserProfiles()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            SELECT Id, Name, Email, DateCreated, ImageUrl
                            FROM UserProfile";

                    using (var reader = cmd.ExecuteReader())
                    {
                        var users = new List<UserProfile>();
                        while (reader.Read())
                        {
                            users.Add(new UserProfile()
                            {
                                Id = DbUtils.GetInt(reader, "Id"),
                                Name = DbUtils.GetString(reader, "Name"),
                                Email = DbUtils.GetString(reader, "Email"),
                                DateCreated = DbUtils.GetDateTime(reader, "DateCreated"),


                            });

                            if (!reader.IsDBNull(reader.GetOrdinal("ImageUrl")))
                            {
                                users.Add(new UserProfile()
                                {
                                    ImageUrl = DbUtils.GetString(reader, "ImageUrl")
                                });
                            }
                        }
                        return users;
                    }
                }
            }
        }

        public UserProfile GetUserProfileByIdWithVideos(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                    SELECT up.Id AS UserProfileId, up.Name, up.Email, up.ImageUrl, up.DateCreated AS UserProfileDateCreated,
                                        v.Id AS VideoId, v.Title, v.Description, v.Url, v.DateCreated As VideoDateCreated,
                                        v.UserProfileId AS VideoUserProfileId
                                    FROM UserProfile up
                                    LEFT JOIN Video v ON v.UserProfileId = up.Id
                                    WHERE up.Id = @id";

                    DbUtils.AddParameter(cmd, "@id", id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        List<UserProfile> profiles = new List<UserProfile>();
                        UserProfile currentUserProfile = null;
                        while(reader.Read())
                        {
                            var userProfileId = DbUtils.GetInt(reader, "UserProfileId");
                            currentUserProfile = profiles.FirstOrDefault(p => p.Id == userProfileId);
                            if (currentUserProfile == null)
                            {
                                currentUserProfile = new UserProfile()
                                {
                                    Id = userProfileId,
                                    Name = DbUtils.GetString(reader, "Name"),
                                    Email = DbUtils.GetString(reader, "Email"),
                                    DateCreated = DbUtils.GetDateTime(reader, "UserProfileDateCreated"),
                                    Videos = new List<Video>()
                                };

                                if (!reader.IsDBNull(reader.GetOrdinal("ImageUrl")))
                                {
                                    profiles.Add(new UserProfile()
                                    {
                                        ImageUrl = DbUtils.GetString(reader, "ImageUrl")
                                    });
                                }
                                profiles.Add(currentUserProfile);
                            }

                            if(DbUtils.IsNotDbNull(reader, "VideoId"))
                            {
                                currentUserProfile.Videos.Add(new Video()
                                {
                                    Id = DbUtils.GetInt(reader, "VideoId"),
                                    Title = DbUtils.GetString(reader, "Title"),
                                    Description = DbUtils.GetString(reader, "Description"),
                                    DateCreated = DbUtils.GetDateTime(reader, "VideoDateCreated"),
                                    Url = DbUtils.GetString(reader, "Url"),
                                    UserProfileId = DbUtils.GetInt(reader, "VideoUserProfileId")
                                });

                            }

                        }
                        return currentUserProfile;
                    }
                }
            }
        }

        public UserProfile GetUserProfileById(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                SELECT Name, Email, DateCreated, ImageUrl
                                FROM UserProfile
                                WHERE Id = @id";

                    DbUtils.AddParameter(cmd, "Id", id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        UserProfile profile = null;
                        if (reader.Read())
                        {
                            profile = new UserProfile()
                            {
                                Id = id,
                                Name = DbUtils.GetString(reader, "Name"),
                                Email = DbUtils.GetString(reader, "Email"),
                                DateCreated = DbUtils.GetDateTime(reader, "DateCreated"),

                            };

                            if (!reader.IsDBNull(reader.GetOrdinal("ImageUrl")))
                            {
                                profile.ImageUrl = DbUtils.GetString(reader, "ImageUrl");
                            }
                        }
                        return profile;
                    }
                }
            }
        }

        public void Add(UserProfile userProfile)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO UserProfile
                                        (Name, Email, DateCreated, ImageUrl)
                                        OUTPUT INSERTED.ID
                                        VALUES (@name, @email, @dateCreated, @imageUrl)";
                    DbUtils.AddParameter(cmd, "@name", userProfile.Name);
                    DbUtils.AddParameter(cmd, "@email", userProfile.Email);
                    DbUtils.AddParameter(cmd, "@dateCreated", userProfile.DateCreated);
                    DbUtils.AddParameter(cmd, "@imageUrl", userProfile.ImageUrl == null ? DBNull.Value : userProfile.ImageUrl);

                    userProfile.Id = (int)cmd.ExecuteScalar();
                }
            }
        }

        public void Update(UserProfile userProfile)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                    UPDATE UserProfile
                                    SET Name = @name,
                                        Email = @email,
                                        DateCreated = @dateCreated,
                                        ImageUrl = @imageUrl
                                    WHERE Id = @id";

                    DbUtils.AddParameter(cmd, "@id", userProfile.Id);
                    DbUtils.AddParameter(cmd, "@name", userProfile.Name);
                    DbUtils.AddParameter(cmd, "@email", userProfile.Email);
                    DbUtils.AddParameter(cmd, "@dateCreated", userProfile.DateCreated);
                    DbUtils.AddParameter(cmd, "@imageUrl", userProfile.ImageUrl == null ? DBNull.Value : userProfile.ImageUrl);

                    cmd.ExecuteNonQuery();

                }
            }
        }

        public void Delete(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"DELETE FROM UserProfile 
                                        WHERE Id = @id";
                    DbUtils.AddParameter(cmd, "@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
