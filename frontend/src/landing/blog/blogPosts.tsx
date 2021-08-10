import { BlogPosts } from "@Palavyr-Types";
import { Post1 } from "./posts/post1/Post1";
import { Post2 } from "./posts/post2/Post2";
import Post1Img from "./posts/post1/post1.png";
import Post2Img from "./posts/post2/post2.png";


export const blogPosts: BlogPosts = [
    {
        title: "Welcome to the world of Palavyr",
        id: 1,
        date: 1628477895,
        src: Post1Img,
        snippet: "Palavyr Kicks off in 2021!",
        content: Post1,
    },
    {
        title: "How do I place the Palavyr Widget into my website?",
        id: 2,
        date: 1628579705,
        src: Post2Img,
        snippet: "After you've spent a lot of hard work getting your chat bot ready, its time to put it in your website. But how?",
        content: Post2,
    },
];
