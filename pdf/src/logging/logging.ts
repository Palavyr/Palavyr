import { isDevelopmentStage } from '../config/currentEnvironment';

export const logDebug = (content: any) => {
    if (isDevelopmentStage()) {
        console.log("DEBUG: " + content);
    }
};

export const logTrace = (content: any) => {
    console.log("TRACE: - " + content);
};
