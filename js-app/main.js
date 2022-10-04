const url = "https://localhost:5001/api/video/";

const button = document.querySelector("#run-button");
button.addEventListener("click", () => {
    getAllVideoVarieties()
        .then(videoVarieties => {
            console.log(videoVarieties);
        })
});

function getAllVideoVarieties() {
    return fetch(url).then(resp => resp.json());
}