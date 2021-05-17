import { logDebug } from 'logging/logging';
import path, { sep } from 'path';

const resolvePathToPhantom = () => {
    const localPath = path.join(resolveCurrentDirectory(), 'phantomjs.exe');
    logDebug('PhantomPath: ' + localPath);
    return localPath;
};

const resolvePathToScript = () => {
    const localPath = path.join(resolveCurrentDirectory(), 'create_script.js');
    logDebug('Script path: ' + localPath);
    return localPath;
};

const resolveCurrentDirectory = (): string => {
    return __filename.split(sep).slice(0, -1).join(sep);
};

export const pathToPhantom = resolvePathToPhantom();
export const pathToScript = resolvePathToScript();
