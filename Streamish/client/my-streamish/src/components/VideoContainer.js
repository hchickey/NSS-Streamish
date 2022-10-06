import { useState } from "react"
import VideoList from "./VideoList"
import { VideoSearch } from "./VideoSearch"



export const VideoContainer = () => {
    const [searchTerm, setSearchTerm] = useState("")

    return <>
        <VideoSearch setterFunction={setSearchTerm} />
        <VideoList searchTermState={searchTerm} />
    </>
}