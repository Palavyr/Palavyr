const MiniCssExtractPlugin = require("mini-css-extract-plugin");

const common = require("./widget-webpack.common.js");
const { merge } = require("webpack-merge");

process.env.NODE_ENV = mode;

module.exports = () => {
    return merge(common('production'), {
        mode,
        devtool: "inline-source-map", // Don't really need this source map
        plugins: [
            new MiniCssExtractPlugin({
                filename: "styles.css",
                chunkFileName: "[id].css",
            }),
        ],
        optimization: {
            minimizer: [
                new OptimizeCSSAssetsPlugin({}),
            ],
        },
    });
};
