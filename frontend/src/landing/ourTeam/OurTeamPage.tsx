import { OurTeamTitleContent } from "@landing/branding/headerTitleContent/OurTeamTitleContent";
import { LandingWrapper } from "@landing/components/LandingWrapper";
import React from "react";
import { OurTeamContent } from "./OurTeamContent";

export const OurTeamPage = () => {
    return <LandingWrapper TitleContent={<OurTeamTitleContent />} MainContent={<OurTeamContent />} />;
};
