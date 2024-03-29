const path = require("path");
const { CleanWebpackPlugin } = require("clean-webpack-plugin");
const HtmlWebpackPlugin = require("html-webpack-plugin");
const TsconfigPathsPlugin = require("tsconfig-paths-webpack-plugin");
const { WebpackManifestPlugin } = require('webpack-manifest-plugin');
const ForkTsCheckerWebpackPlugin = require("fork-ts-checker-webpack-plugin");
const Dotenv = require("dotenv-webpack");
const { TypeScriptLoaderRule, BabelLoaderRule, StylesLoader, URLLoaderRule, SVGRLoader, ScssLoaderRule } = require("./webpack/rules-frontend");
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const CopyPlugin = require("copy-webpack-plugin");

module.exports = ENV => {

    const envPath = ENV === "production" ? ".env.frontend.production" : ".env.frontend.development";
    console.log("Building in.... " + envPath);
    return {
        entry: {
            "palavyr-build": "./src/frontend/index.tsx",
        },
        plugins: [
            new MiniCssExtractPlugin(),
            new Dotenv({ path: envPath }),
            new CleanWebpackPlugin(), //for < v2 versions of CleanWebpackPlugin
            new WebpackManifestPlugin({
                fileName: "manifest.json",
            }),
            new HtmlWebpackPlugin({
                template: "./public/index.html",
                filename: "index.html",
                title: "Palavyr Configuration Portal",
            }),
            new ForkTsCheckerWebpackPlugin(),
            new CopyPlugin({
                patterns: [
                    { from: "./public/favicon.ico" },
                    { from: "./public/favicon-16x16.png" },
                    { from: "./public/favicon-32x32.png" },
                    { from: "./public/apple-touch-icon.png" },
                    { from: "./public/android-chrome-192x192.png" },
                    { from: "./public/android-chrome-512x512.png" },
                    { from: "./public/MAINTENANCE.html" },
                ],
            }),
        ],
        output: {
            filename: "[name].bundle.js",
            path: path.resolve(__dirname, "./dist-frontend"),
            publicPath: "/",
        },
        resolve: {
            plugins: [new TsconfigPathsPlugin()],
            extensions: [".tsx", ".ts", ".js"],
        },
        module: {
            rules: [
                TypeScriptLoaderRule(), // IN USE
                StylesLoader(), // IN USE
                SVGRLoader(),
                BabelLoaderRule(), // IN USE
                URLLoaderRule(), // IN USE
                ScssLoaderRule(),
                //new rule here
            ],
        },
    };
};
