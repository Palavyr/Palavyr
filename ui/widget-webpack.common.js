const path = require("path");
const TsconfigPathsPlugin = require("tsconfig-paths-webpack-plugin");
const ManifestPlugin = require("webpack-manifest-plugin");
const ForkTsCheckerWebpackPlugin = require("fork-ts-checker-webpack-plugin");
const Dotenv = require("dotenv-webpack");
const HtmlWebpackPlugin = require("html-webpack-plugin");
const { CleanWebpackPlugin } = require("clean-webpack-plugin");
const CopyPlugin = require("copy-webpack-plugin");
const autoprefixer = require("autoprefixer");

const webpack = require("webpack");

module.exports = envPath => {
    console.log("Building in: " + envPath);
    return {
        plugins: [
            new Dotenv({ path: envPath }),
            new ForkTsCheckerWebpackPlugin(),
            new HtmlWebpackPlugin({
                template: "./public/index.html",
                filename: "index.html",
                favicon: "public/favicon.ico",
                title: "Palavyr Widget",
            }),
            new webpack.ProvidePlugin({
                React: "react",
            }),
            new ManifestPlugin({
                fileName: "manifest.json",
            }),
            new CleanWebpackPlugin(),
            new CopyPlugin({
                patterns: [
                    { from: "./public/favicon.ico" },
                    { from: "./public/favicon-16x16.png" },
                    { from: "./public/favicon-32x32.png" },
                    { from: "./public/apple-touch-icon.png" },
                    { from: "./public/android-chrome-192x192.png" },
                    { from: "./public/android-chrome-512x512.png" },
                    { from: "./public/background-gif.mp4" },
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
        module: {
            rules: [
                {
                    test: /\.ts(x?)$/,
                    exclude: /node_modules/,
                    use: ["babel-loader", "ts-loader"],
                },
                {
                    enforce: "pre",
                    test: /\.js$/,
                    loader: "source-map-loader",
                },
                {
                    test: /\.js$/,
                    loader: "babel-loader",
                    exclude: /node_modules/,
                },
                {
                    test: /\.scss$/,
                    exclude: /node_modules/,
                    use: [
                        "style-loader",
                        "css-loader",
                        {
                            loader: "postcss-loader",
                            options: {
                                ident: "postcss",
                                plugins: () => [
                                    require("postcss-flexbugs-fixes"), // eslint-disable-line
                                    autoprefixer({
                                        browsers: [">1%", "last 4 versions", "Firefox ESR", "not ie <9"],
                                        flexbox: "no-2009",
                                    }),
                                ],
                            },
                        },
                        "sass-loader",
                    ],
                },
                {
                    test: /\.(jpg|png|gif|svg)$/,
                    use: "url-loader",
                },
            ],
        },
    };
};
