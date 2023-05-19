const MiniCssExtractPlugin = require("mini-css-extract-plugin");

const common = require("./widget-webpack.common.js");
const { merge } = require("webpack-merge");

module.exports = () => {
    return merge(common('production'), {
        mode: 'production',
        devtool: "inline-source-map", // Don't really need this source map
        plugins: [
            new MiniCssExtractPlugin({
                filename: "styles.css",
                chunkFilename: "[id].css",
            }),
        ],
        optimization: {
            minimizer: [
                new OptimizeCSSAssetsPlugin({}),
            ],
        },
    });
};
