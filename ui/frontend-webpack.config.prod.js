const { merge } = require("webpack-merge");
const common = require("./frontend-webpack.common.js");

module.exports = (ENV) => {
    return merge(common(ENV), {
        mode: "production",
        devtool: "inline-source-map",

        // devtool: 'source-map' // Don't really need this source map
    });
};