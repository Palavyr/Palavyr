const slsw = require('serverless-webpack');
const path = require('path');
const { CleanWebpackPlugin } = require('clean-webpack-plugin');
const TsconfigPathsPlugin = require('tsconfig-paths-webpack-plugin');
const ForkTsCheckerWebpackPlugin = require('fork-ts-checker-webpack-plugin');
const Dotenv = require('dotenv-webpack');
const { TypeScriptLoaderRule, BabelLoaderRule } = require('./webpack/rules');
const CopyPlugin = require('copy-webpack-plugin');

module.exports = {
    // https://stackoverflow.com/questions/31102035/how-can-i-use-webpack-with-express (target: 'Node' in webpack)
    target: 'node',
    entry: slsw.lib.entries,
    mode: 'none',
    devtool: 'inline-source-map',
    plugins: [
        new Dotenv({ path: '.env.development' }),
        new CleanWebpackPlugin(), //for < v2 versions of CleanWebpackPlugin
        new ForkTsCheckerWebpackPlugin(),
        new CopyPlugin({
            patterns: [
                { from: './assets/create_script.js', to: './create_script.js' },
                { from: './assets/phantomjs.exe', to: './phantomjs.exe' },
            ],
        }),
    ],
    output: {
        filename: 'lambda.js',
        path: path.resolve(__dirname, './dist'),
        publicPath: '/',
    },
    resolve: {
        plugins: [new TsconfigPathsPlugin()],
        extensions: ['.ts', '.js'],
    },
    module: {
        exprContextCritical: false,
        rules: [TypeScriptLoaderRule(), BabelLoaderRule()],
    },
};
