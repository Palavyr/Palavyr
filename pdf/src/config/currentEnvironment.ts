import { logDebug } from 'logging/logging';

const isWindows = process.platform.startsWith('win');
if (isWindows) {
    if (process.env.NODE_ENV !== 'development' && process.env.NODE_ENV !== 'production') {
        // @ts-ignore: no assign to readonly
        process.env.NODE_ENV = 'development';
    }
} else {
    if (process.env.NODE_ENV !== 'development' && process.env.NODE_ENV !== 'production') {
        // @ts-ignore: no assign to readonly
        process.env.NODE_ENV = 'production';
    }
}

export const currentEnvironment = process.env.NODE_ENV as string;

if (isWindows && currentEnvironment === undefined) {
    // logDebug('CURRENTENV not set');
    throw new Error('Must Set Node Env during build.');
}

export const isDevelopmentStage = () => {
    return isWindows; // This is TEMPORARY _ just here for Paul's setup until this bit is worked out (later)
    // if (isWindows) {
    //     return true;
    //     // return currentEnvironment.toUpperCase() === 'development'.toUpperCase();
    // } else {
    //     return false;
    // }
};
