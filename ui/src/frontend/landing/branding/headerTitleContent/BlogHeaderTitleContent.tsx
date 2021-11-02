import { TitleContent } from "@landing/components/TitleContent";
import React from "react";
import { TitleTypography } from "./components/TitleTypography";

export const BlogTitleHeaderContent = () => {
    return (
        <>
            <TitleContent title={<TitleTypography>The Palavyr Blog</TitleTypography>} />
        </>
    );
};
