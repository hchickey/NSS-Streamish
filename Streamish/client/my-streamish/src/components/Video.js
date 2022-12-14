import React from "react";
import { Card, CardBody } from "reactstrap";

const Video = ({ video }) => {

    const Comments = () => {
        return <ul>
            {
                video?.comments?.map((comment) => {
                    return <li key={comment.id}>{comment?.message}</li>
                })
            }
        </ul>
    }

    return (
        <Card >
            <p className="text-left px-2">Posted by: {video.userProfile.name}</p>
            <CardBody>
                <iframe className="video"
                    src={video.url}
                    title="YouTube video player"
                    frameBorder="0"
                    allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture"
                    allowFullScreen />

                <p>
                    <strong>{video.title}</strong>
                </p>
                <p>{video.description}</p>
                <div>Comments: {Comments()}</div>
            </CardBody>
        </Card>
    );
};

export default Video;