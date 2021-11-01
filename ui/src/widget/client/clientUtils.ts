export const serverUrl = process.env.REACT_APP_API_URL as string;
export const googleAnalyticsTrackingId = process.env.GOOGLE_ANALYTICS_KEY as string;
export const currentEnvironment = process.env.CURRENTENV as string;

export const isDevelopmentStage = () => {
    return currentEnvironment.toUpperCase() !== "Production".toUpperCase();
};
