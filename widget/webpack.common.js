const path = require('path');
const { createRules } = require('./webpack/rules');
const TsconfigPathsPlugin = require('tsconfig-paths-webpack-plugin');

module.exports = ENV => {
    const envPath = ENV.production ? '.env.production' : '.env.development';
    console.log('Building in: ' + envPath);

    return {
        entry: {
            'palavyr-widget-build': './src/index.tsx',
        },
        output: {
            filename: '[name].bundle.js',
            path: path.resolve(__dirname, './dist'),
            publicPath: '/',
        },
        resolve: {
            plugins: [new TsconfigPathsPlugin()],
            extensions: ['.tsx', '.ts', '.js'],
        },
        module: { rules: createRules() },
    };
};
