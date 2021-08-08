import { BlogPostRecord, BlogPosts } from "@Palavyr-Types";
import React from "react";
import { Route, Switch } from "react-router-dom";
import { BlogPage } from "./BlogPage";
import { BlogPost } from "./components/BlogPost";
import useLocationBlocker from "./components/utils/useLocationBlocker";

export interface BlogRouteProps {
    blogPosts: BlogPosts;
}

const convertTitleToUriCompatible = (rawTitle: string) => {
    let title = rawTitle;
    title = title.toLowerCase();
    /* Remove unwanted characters, only accept alphanumeric and space */
    title = title.replace(/[^A-Za-z0-9 ]/g, "");
    /* Replace multi spaces with a single space */
    title = title.replace(/\s{2,}/g, " ");
    /* Replace space with a '-' symbol */
    title = title.replace(/\s/g, "-");
    return title;
};

const createPostUrl = (title: string) => {
    return `/blog/post/${title}`;
};

const createPostParam = (id: number) => {
    return `?id=${id}`;
};

export type BlogPostRouteMeta = BlogPostRecord & {
    url: string;
    params: string;
};

export const BlogRoutes = ({ blogPosts }: BlogRouteProps) => {
    useLocationBlocker();

    const blogPostRouteMetas: BlogPostRouteMeta[] = blogPosts.map((blogPost) => {
        const titleSlug = convertTitleToUriCompatible(blogPost.title);
        const url = createPostUrl(titleSlug);
        const params = createPostParam(blogPost.id);

        return {
            ...blogPost,
            url,
            params,
        };
    });

    return (
        <Switch>
            {blogPostRouteMetas.map((post: BlogPostRouteMeta) => {
                return <Route exact path={post.url} render={() => <BlogPost date={post.date} title={post.title} src={post.url} content={post.content} otherArticles={blogPostRouteMetas.filter((m) => m.id !== post.id)} />} />;
            })}
            <Route exact path="/blog" render={() => <BlogPage blogPosts={blogPostRouteMetas}  />} />
        </Switch>
    );
};
