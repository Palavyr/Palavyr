import { webUrl } from "@api-client/clientUtils";
import { BlogPosts } from "@Palavyr-Types";
import { Post1 } from "./posts/Post1";
import { Post2 } from "./posts/Post2";

export const blogPosts: BlogPosts = [
    {
        title: "Welcome to the world of Palavyr",
        id: 1,
        date: 1628477895,
        src: `${webUrl}/public/blogImages/post1/post1.png`,
        snippet: "Palavyr Kicks off in 2021!",
        content: Post1,
    },
    {
        title: "What problem does Palavyr solve?",
        id: 2,
        date: 1628477895,
        src: `${webUrl}/public/blogImages/post2/blogPost2.jpg`,
        snippet: "We often get asked the question: What problem does Palavyr actually solve? And we have a great answer.",
        content: Post2,
    },
];
