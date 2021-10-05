import { logDebug } from '../logging/logging';

const isWindows = process.platform.startsWith('win');
if (isWindows) {
    process.env.NODE_ENV = 'development';
} else {
    process.env.NODE_ENV = 'production';
}

export const currentEnvironment = process.env.NODE_ENV as string;

if (isWindows && currentEnvironment === undefined) {
    // logDebug('CURRENTENV not set');
    throw new Error('Must Set Node Env during build.');
}

export const isDevelopmentStage = () => {
    if (isWindows) {
        return true;
        // return currentEnvironment.toUpperCase() === 'development'.toUpperCase();
    } else {
        return false;
    }
};
