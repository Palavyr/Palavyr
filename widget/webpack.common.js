const path = require("path");
const TsconfigPathsPlugin = require("tsconfig-paths-webpack-plugin");
const ManifestPlugin = require('webpack-manifest-plugin');
const ForkTsCheckerWebpackPlugin = require('fork-ts-checker-webpack-plugin');
const Dotenv = require('dotenv-webpack');
const HtmlWebpackPlugin = require('html-webpack-plugin');
const CleanWebpackPlugin = require("clean-webpack-plugin");

const webpack = require("webpack");

const { createRules } = require("./webpack/rules");
const { manifestOptions, htmlOptions } = require(path.resolve(__dirname, './webpack/options'));

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
        ],
        entry: {
            "palavyr-widget-build": "./src/index.tsx",
        },
        output: {
            filename: "[name].bundle.js",
            path: path.resolve(__dirname, "./dist"),
            publicPath: "/",
        },
        resolve: {
            plugins: [new TsconfigPathsPlugin()],
            extensions: [".tsx", ".ts", ".js"],
        },
        module: { rules: createRules() },
    };
};
