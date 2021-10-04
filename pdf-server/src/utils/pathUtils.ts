import { logDebug } from '../logging/logging';
import path, { sep } from 'path';

const windowsExecutable = 'phantomjs-windows.exe';
const linuxExecutable = 'phantomjs-linux';

const resolvePathToPhantom = () => {
    const isWindows = process.platform.startsWith('win');
    logDebug('OS: ' + isWindows ? 'windows' : 'linux');
    const localPath = path.join(resolveCurrentDirectory(), isWindows ? windowsExecutable : linuxExecutable);
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
