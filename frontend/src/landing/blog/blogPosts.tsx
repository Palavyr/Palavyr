import { BlogPosts } from "@Palavyr-Types";
import { Post1 } from "./posts/Post1";
import { Post2 } from "./posts/Post2";


export const blogPosts: BlogPosts = [
    {
        title: "Post 1",
        id: 1,
        date: 1576281600,
        src: `${process.env.PUBLIC_URL}/images/logged_out/blogPost1.jpg`,
        snippet: "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua.",
        content: Post1,
    },
    {
        title: "Post 2",
        id: 2,
        date: 1576391600,
        src: `${process.env.PUBLIC_URL}/images/logged_out/blogPost2.jpg`,
        snippet: "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua.",
        content: Post2,
    },
];
