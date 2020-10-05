const path = require('path');
const { CleanWebpackPlugin } = require('clean-webpack-plugin');
const HtmlWebpackPlugin = require('html-webpack-plugin');
const { manifestOptions, htmlOptions } = require(path.resolve(__dirname, './webpack/options'));
const TsconfigPathsPlugin = require('tsconfig-paths-webpack-plugin');
const ManifestPlugin = require('webpack-manifest-plugin');
const ForkTsCheckerWebpackPlugin = require('fork-ts-checker-webpack-plugin');
const Dotenv = require('dotenv-webpack');
const { TypeScriptLoaderRule, FileLoaderRule, SVGRule, BabelLoaderRule, StylesLoader, URLLoaderRule, CSSMinify } = require("./webpack/rules");
const webpack = require('webpack');
const MiniCssExtractPlugin = require('mini-css-extract-plugin');


module.exports = (ENV) => {

    const envPath = ENV.production ? ".env.production" : ".env.development";
    console.log("Building in.... " + envPath)
    return {
        entry: {
            "palavyr-build": './src/index.tsx',
        },
        plugins: [
            new MiniCssExtractPlugin(),
            new Dotenv({ path: envPath }),
            new CleanWebpackPlugin(), //for < v2 versions of CleanWebpackPlugin
            new HtmlWebpackPlugin({ title: 'Palavyr Prod' }),
            new ManifestPlugin(manifestOptions),
            new HtmlWebpackPlugin(htmlOptions),
            new ForkTsCheckerWebpackPlugin(),
        ],
        output: {
            filename: '[name].bundle.js',
            path: path.resolve(__dirname, './dist'),
            publicPath: "/"
        },
        resolve: {
            plugins: [new TsconfigPathsPlugin()],
            extensions: ['.tsx', '.ts', '.js'],
        },
        module: {
            rules: [
                TypeScriptLoaderRule(),
                StylesLoader(),
                FileLoaderRule(),
                SVGRule(),
                BabelLoaderRule(),
                URLLoaderRule(),
                //new rule here
            ],
        },
       
    }
}