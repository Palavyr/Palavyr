import { port } from './http/port';
import { startServer } from './server/startServer';
import app from './app';

startServer(app, port);
