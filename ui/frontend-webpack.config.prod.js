const { merge } = require("webpack-merge");
const common = require("./frontend-webpack.common.js");

module.exports = (_) => {
    return merge(common('production'), {
        mode: "production",
        devtool: "inline-source-map",
        // devtool: 'source-map' // Don't really need this source map
    });
};
