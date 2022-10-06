import React, { useEffect, useState } from "react";
import Video from './Video';
import { getAllVideosWithComments } from "../modules/videoManager";

const VideoList = ({ searchTermState }) => {
    const [videos, setVideos] = useState([]);
    // const [comments, setComments] = useState([]);


    const getVideos = () => {
        getAllVideosWithComments().then(videos => setVideos(videos));
    };

    useEffect(() => {
        getVideos();
    }, []
    );

    useEffect(() => {
        searchVideos(searchTermState)
    }, [searchTermState]
    );

    const searchVideos = (searchTerm) => {
        fetch(`/api/video/search?q=${searchTerm}&sortDesc=false`)
            .then(res => res.json())
            .then((videos) => {
                setVideos(videos)
            })
    }

    return (
        <div className="container">
            <div className="row justify-content-center">
                {videos.map((video) => (
                    <Video video={video} key={video.id} />
                ))}
            </div>
        </div>
    );
};

export default VideoList;