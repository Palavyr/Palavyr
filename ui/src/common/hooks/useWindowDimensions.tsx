import { useState, useEffect } from "react";

export const getWindowsDimensions = () => {
    const { innerWidth: width, innerHeight: height } = window;
    return { width, height };
};

export const useWindowDimensions = () => {
    const [windowDimensions, setWindowDimensions] = useState(getWindowsDimensions());

    useEffect(() => {
        const handleResize = () => {
            setWindowDimensions(getWindowsDimensions());
        };

        window.addEventListener("resize", handleResize);

        return () => {
            window.removeEventListener("resize", handleResize);
        };
    }, []);

    return windowDimensions;
};
