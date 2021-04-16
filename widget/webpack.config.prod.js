const OptimizeCSSAssetsPlugin = require("optimize-css-assets-webpack-plugin");
const UglifyJsPlugin = require("uglifyjs-webpack-plugin");
const MiniCssExtractPlugin = require("mini-css-extract-plugin");

const common = require("./webpack.common.js");
const { merge } = require("webpack-merge");

const mode = "production";

process.env.NODE_ENV = mode;

module.exports = () => {
    return merge(common(`.env.${mode}`), {
        devtool: "inline-source-map", // Don't really need this source map
        plugins: [
            new MiniCssExtractPlugin({
                filename: "styles.css",
                chunkFileName: "[id].css",
            }),
        ],
        optimization: {
            minimizer: [
                new UglifyJsPlugin({
                    cache: true,
                    parallel: true,
                }),
                new OptimizeCSSAssetsPlugin({}),
            ],
        },
    });
};
