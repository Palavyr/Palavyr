import { OurStoryPageTitleContent } from "@landing/branding/headerTitleContent/OurStoryPageTitleContent";
import { LandingWrapper } from "@landing/components/LandingWrapper";
import React from "react";
import { OurStoryContent } from "./components/OurStoryContent";
import { OurStoryWrapper } from "./components/OurStoryWrapper";

export const OurStoryPage = () => {
    return (
        <LandingWrapper
            TitleContent={<OurStoryPageTitleContent />}
            MainContent={
                <OurStoryWrapper>
                    <OurStoryContent />
                </OurStoryWrapper>
            }
        />
    );
};
