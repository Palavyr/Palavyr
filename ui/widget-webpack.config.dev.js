const webpack = require("webpack");
const path = require("path");
const common = require("./widget-webpack.common.js");
const { merge } = require("webpack-merge");

process.env.NODE_ENV = mode;

module.exports = () => {
    return merge(common('development'), {
        'development',
        devServer: {
            contentBase: path.resolve(__dirname, "dist"),
            historyApiFallback: true,
            compress: false,
            port: 3400,
            hot: true,
        },
        devtool: "inline-source-map",
        plugins: [
            new webpack.HotModuleReplacementPlugin(),
        ],
        performance: {
            hints: false,
        },
    });
};
