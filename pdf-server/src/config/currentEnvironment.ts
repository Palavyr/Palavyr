import { logDebug } from '../logging/logging';

export const currentEnvironment = process.env.NODE_ENV as string;

if (currentEnvironment === undefined) {
    // logDebug('CURRENTENV not set');
    // throw new Error('Must Set Node Env during build.');
}

export const isDevelopmentStage = () => {
    return true;
    // return currentEnvironment.toUpperCase() === 'development'.toUpperCase();
};
