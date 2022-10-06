const baseUrl = '/api/video';

export const getAllVideos = () => {
    return fetch(baseUrl)
        .then((res) => res.json())
};

export const addVideo = (video) => {
    return fetch(baseUrl, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify(video),
    });
};

const baseUrlTwo = '/api/video/getWithComments';


export const getAllVideosWithComments = () => {
    return fetch(baseUrlTwo)
        .then((res) => res.json())
};

export const addVideoWithComments = (comments) => {
    return fetch(baseUrlTwo, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify(comments),
    });
};

const searchUrl = '/api/video/search';

export const searchVideos = () => {
    return fetch(searchUrl).then((res) => res.json())
};