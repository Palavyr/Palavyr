const path = require('path');
const { CleanWebpackPlugin } = require('clean-webpack-plugin');
const TsconfigPathsPlugin = require('tsconfig-paths-webpack-plugin');
const ForkTsCheckerWebpackPlugin = require('fork-ts-checker-webpack-plugin');
const Dotenv = require('dotenv-webpack');
const { TypeScriptLoaderRule, BabelLoaderRule } = require('./webpack/rules');
var nodeExternals = require('webpack-node-externals');


//TODO: https://webpack.js.org/plugins/npm-install-webpack-plugin/

module.exports = (ENV) => {

    const envPath = ENV === 'production' ? '.env.production' : '.env.development';
    console.log('Building in.... ' + envPath)

    return {
        entry: {
            'palavyr-pdf': './src/app.ts',
        },

        // https://stackoverflow.com/questions/31102035/how-can-i-use-webpack-with-express (target: 'Node' in webpack)
        target: 'node',

        plugins: [
            new Dotenv({ path: envPath }),
            // new CleanWebpackPlugin(), //for < v2 versions of CleanWebpackPlugin
            // new ForkTsCheckerWebpackPlugin(),
        ],
        output: {
            filename: '[name].server.js',
            path: path.resolve(__dirname, './dist'),
            publicPath: '/'
        },
        resolve: {
            plugins: [new TsconfigPathsPlugin()],
            extensions: ['.ts', '.js']
        },
        module: {
            exprContextCritical: false,
            rules: [
                TypeScriptLoaderRule(), // IN USE
                BabelLoaderRule() // IN USE
            ],
        },
    }
}