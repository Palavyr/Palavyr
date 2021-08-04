import { Dialog } from "@material-ui/core";
import React from "react";

export type TourStep = {
    selector: string;
};

export const MaterialTour = () => {
    const steps: TourStep[] = [
        {
            selector: "my-state",
        },
    ];

    return (
        <>
            {steps.filter((step: TourStep, index: number) => {
                const element = document.getElementById(step.selector) as HTMLElement;
                const elementRect = element.getBoundingClientRect();

                let offsetX = window.pageXOffset;
                let offsetY = window.pageYOffset;

                const x0 = elementRect.left + elementRect.width + offsetX;
                const y0 = elementRect.top + elementRect.height + offsetY;

                // return <Dialog></Dialog>
            })}
        </>
    );
};
