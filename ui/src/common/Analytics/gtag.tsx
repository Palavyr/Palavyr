export const pageview = (url: string) => {
    const code = process.env.NEXT_PUBLIC_GOOGLE_ANALYTICS as string;
    window.gtag("config", code, {
        page_path: url,
    });
};

// log specific events happening.
export const event = ({ action, params }) => {
    window.gtag("event", action, params);
};
