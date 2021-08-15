import React, { useEffect } from "react";
import classNames from "classnames";
import format from "date-fns/format";
import { Grid, Typography, Card, Box, makeStyles } from "@material-ui/core";
import smoothScrollTop from "./utils/smoothScroll";
import { ZoomImage } from "@common/components/borrowed/ZoomImage";
import { ShareButton } from "./shareButtons/ShareButton";
import { BlogCard } from "./BlogCard";
import { LandingWrapper } from "@landing/components/LandingWrapper";
import { BlogTitleHeaderContent } from "@landing/branding/headerTitleContent/BlogHeaderTitleContent";
import { BlogPostRouteMeta } from "@Palavyr-Types";

const useStyles = makeStyles(theme => ({
    blogContentWrapper: {
        marginLeft: theme.spacing(1),
        marginRight: theme.spacing(1),
        marginBottom: theme.spacing(4),
        [theme.breakpoints.up("sm")]: {
            marginLeft: theme.spacing(4),
            marginRight: theme.spacing(4),
        },
        maxWidth: 1280,
        width: "100%",
    },
    wrapper: {
        minHeight: "60vh",
        paddingTop: "3rem",
    },
    img: {
        width: "100%",
        height: "auto",
    },
    card: {
        boxShadow: theme.shadows[4],
        width: "100%"
    },
}));

export interface BlogPostProps {
    date: number;
    title: string;
    url: string;
    img: string;
    content: React.ReactNode;
    otherArticles: BlogPostRouteMeta[];
}
export const BlogPost = ({ date, title, url, img, content, otherArticles }: BlogPostProps) => {
    const cls = useStyles();
    useEffect(() => {
        document.title = `Palavyr - ${title}`;
        smoothScrollTop();
    }, [title]);

    return (
        <LandingWrapper
            TitleContent={<BlogTitleHeaderContent />}
            MainContent={
                <Box className={classNames(cls.wrapper)} display="flex" justifyContent="center">
                    <div className={cls.blogContentWrapper}>
                        <Grid container spacing={5}>
                            <Grid item md={9}>
                                <Card className={cls.card}>
                                    <Box pt={3} pr={3} pl={3} pb={2}>
                                        <Typography variant="h4">
                                            <b>{title}</b>
                                        </Typography>
                                        <Typography variant="body1" color="textSecondary">
                                            {format(new Date(date * 1000), "PPP", {
                                                // awareOfUnicodeTokens: true,
                                            })}
                                        </Typography>
                                    </Box>
                                    <ZoomImage className={cls.img} imgSrc={img} alt="" />
                                    <Box p={3}>
                                        {content}
                                        <Box pt={2}>
                                            <Grid spacing={1} container>
                                                {["Facebook", "Twitter", "Reddit", "Tumblr"].map((type, index) => (
                                                    <Grid item key={index}>
                                                        <ShareButton type={type} title={title} description="I found an awesome article about Palavyr!" disableElevation={true} variant="contained" />
                                                    </Grid>
                                                ))}
                                            </Grid>
                                        </Box>
                                    </Box>
                                </Card>
                            </Grid>
                            <Grid item md={3}>
                                <Typography variant="h6" paragraph>
                                    Other articles
                                </Typography>
                                {otherArticles.length > 0 ? (
                                    <>
                                        {otherArticles.map(blogPost => (
                                            <Box key={blogPost.id} mb={3}>
                                                <BlogCard title={blogPost.title} src={blogPost.src} snippet={blogPost.snippet} date={blogPost.date} url={`${blogPost.url}${blogPost.params}`} />
                                            </Box>
                                        ))}
                                    </>
                                ) : (
                                    <Typography>No other articles yet</Typography>
                                )}
                            </Grid>
                        </Grid>
                    </div>
                </Box>
            }
        />
    );
};
