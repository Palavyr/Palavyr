/* eslint-disable jsx-a11y/alt-text */

const path = require("path");
const TsconfigPathsPlugin = require("tsconfig-paths-webpack-plugin");
const ManifestPlugin = require("webpack-manifest-plugin");
const ForkTsCheckerWebpackPlugin = require("fork-ts-checker-webpack-plugin");
const Dotenv = require("dotenv-webpack");
const HtmlWebpackPlugin = require("html-webpack-plugin");
const { CleanWebpackPlugin } = require("clean-webpack-plugin");
const CopyPlugin = require("copy-webpack-plugin");

const webpack = require("webpack");

const { createRules } = require("./webpack/rules-widget");
const { manifestOptions, htmlOptions } = require(path.resolve(__dirname, "./webpack/options-widget"));

module.exports = envPath => {
    console.log("Building in: " + envPath);
    return {
        plugins: [
            new Dotenv({ path: envPath }),
            new ForkTsCheckerWebpackPlugin(),
            new HtmlWebpackPlugin(htmlOptions),
            new webpack.ProvidePlugin({
                React: "react",
            }),
            new ManifestPlugin(manifestOptions),
            new CleanWebpackPlugin(),
            new CopyPlugin({
                patterns: [
                    { from: "./public/favicon.ico" },
                    { from: "./public/favicon-16x16.png" },
                    { from: "./public/favicon-32x32.png" },
                    { from: "./public/apple-touch-icon.png" },
                    { from: "./public/android-chrome-192x192.png" },
                    { from: "./public/android-chrome-512x512.png" },
                ],
            }),
        ],
        entry: {
            "palavyr-widget-build": "./src/widget/index.tsx",
        },
        output: {
            filename: "[name].bundle.js",
            path: path.resolve(__dirname, "./dist-widget"),
            publicPath: "/",
        },
        resolve: {
            plugins: [new TsconfigPathsPlugin()],
            extensions: [".tsx", ".ts", ".js"],
        },
        module: { rules: createRules() },
    };
};
