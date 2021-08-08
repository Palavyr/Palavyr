import React, { useEffect } from "react";
import classNames from "classnames";
import { Grid, Box, isWidthUp, makeStyles, withWidth, GridSize } from "@material-ui/core";
import BlogCard from "./components/BlogCard";
import { BlogPostRecord } from "@Palavyr-Types";
import { BlogPostRouteMeta } from "./BlogRoutes";
import { Breakpoint } from "@material-ui/core/styles/createBreakpoints";

const useStyles = makeStyles((theme) => ({
    blogContentWrapper: {
        marginLeft: theme.spacing(1),
        marginRight: theme.spacing(1),
        [theme.breakpoints.up("sm")]: {
            marginLeft: theme.spacing(4),
            marginRight: theme.spacing(4),
        },
        maxWidth: 1280,
        width: "100%",
    },
    wrapper: {
        minHeight: "60vh",
    },
    noDecoration: {
        textDecoration: "none !important",
    },
}));

export const getVerticalBlogPosts = (width: Breakpoint, blogPosts: BlogPostRouteMeta[]) => {
    const gridRows: Array<Array<JSX.Element>> = [[], [], []];
    let rows: number;
    let xs: GridSize = 12;
    if (isWidthUp("md", width)) {
        rows = 3;
        xs = 4;
    } else if (isWidthUp("sm", width)) {
        rows = 2;
        xs = 6;
    } else {
        rows = 1;
        xs = 12;
    }
    blogPosts.forEach((blogPost: BlogPostRouteMeta, index: number) => {
        gridRows[index % rows].push(
            <Grid key={blogPost.id} item xs={12}>
                <Box mb={3}>
                    <BlogCard src={blogPost.src} title={blogPost.title} snippet={blogPost.snippet} date={blogPost.date} url={blogPost.url} />
                </Box>
            </Grid>
        );
    });
    return gridRows.map((element: React.ReactNode, index: number) => (
        <Grid key={index} item xs={xs}>
            {element}
        </Grid>
    ));
};

export interface BlogProps {
    width: Breakpoint;
    blogPosts: BlogPostRouteMeta[];
}

export const BlogPage = withWidth()(({ width, blogPosts }: BlogProps) => {
    const cls = useStyles();
    // useEffect(() => {
    //     selectBlog();
    // }, [selectBlog]);

    return (
        <Box display="flex" justifyContent="center" className={classNames(cls.wrapper, "lg-p-top")}>
            <div className={cls.blogContentWrapper}>
                <Grid container spacing={3}>
                    {getVerticalBlogPosts(width, blogPosts)}
                </Grid>
            </div>
        </Box>
    );
});
