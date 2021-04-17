import { CircularProgress, LinearProgress } from "@material-ui/core";
import React from "react";

interface ChatLoadingSpinnerProps {
    loading: boolean;
    spinner?: boolean;
}

export const ChatLoadingSpinner = ({ loading, spinner }: ChatLoadingSpinnerProps) => {
    return spinner ? (
        <div style={{ width: "100%", display: "flex", justifyContent: "center" }}>{loading && <CircularProgress color="primary" />}</div>
    ) : (
        <>{loading && <LinearProgress color="primary" />}</>
    );
};
