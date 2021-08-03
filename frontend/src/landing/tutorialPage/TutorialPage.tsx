import React, { useState, useEffect } from "react";
import { TitleContent } from "@landing/components/TitleContent";
import { Card, makeStyles, Typography } from "@material-ui/core";
import { PalavyrRepository } from "@api-client/PalavyrRepository";
import { VideoMap } from "@Palavyr-Types";
import { LandingWrapper } from "@landing/components/LandingWrapper";

const useStyles = makeStyles((theme) => ({
    wrapper: {
        backgroundColor: theme.palette.common.white,
        overflowX: "hidden",
    },
    primaryText: {
        color: theme.palette.success.main,
    },
    secondaryText: {
        color: theme.palette.success.dark,
    },
    button: {
        width: "18rem",
        alignSelf: "center",
        backgroundColor: theme.palette.background.default,
        color: theme.palette.common.black,
        "&:hover": {
            backgroundColor: theme.palette.success.light,
            color: theme.palette.common.black,
        },
    },
    contentPadding: {
        paddingTop: "2rem",
        paddingBottom: "3rem",
        display: "flex",
        flexDirection: "column",
        justifyContent: "center",
    },
    media: {
        width: "825px",
        height: "508px",
        boxShadow: theme.shadows[10],
    },
    mediaSpan: {
        display: "flex",
        justifyContent: "center",
    },
}));

export const TutorialPage = () => {
    const repository = new PalavyrRepository();
    const [videoMap, setVideoMap] = useState<VideoMap[]>([]);

    useEffect(() => {
        (async () => {
            const currentMap = await repository.Youtube.GetVideoMap();
            setVideoMap(currentMap);
        })();
    }, []);

    const cls = useStyles();

    return (
        <LandingWrapper
            TitleContent={
                <TitleContent
                    title={
                        <Typography align="center" variant="h2" className={cls.primaryText}>
                            Palavyr Getting Started Tutorial series
                        </Typography>
                    }
                    subtitle={
                        <Typography align="center" variant="h6" className={cls.secondaryText}>
                            We are adding more tutorials regularly, so subscribe to our channel to receive updates!
                        </Typography>
                    }
                />
            }
            MainContent={
                <>
                    {videoMap.map((video: VideoMap) => {
                        return (
                            <>
                                <div className={cls.contentPadding}>
                                    <VideoTitle title={video.title} />
                                    <span className={cls.mediaSpan}>
                                        <Card className={cls.media}>
                                            <iframe style={{ height: "100%", width: "100%" }} src={createVideoUrl(video.videoId)} allowFullScreen></iframe>
                                        </Card>
                                    </span>
                                </div>
                            </>
                        );
                    })}
                </>
            }
        ></LandingWrapper>
    );
};

const createVideoUrl = (id: string) => `https://www.youtube.com/embed/${id}`;

export interface IVideoTitle {
    title: string;
}

export const VideoTitle = ({ title }: IVideoTitle) => {
    return (
        <Typography align="center" variant="h3" style={{ padding: "1rem" }}>
            {title}
        </Typography>
    );
};
