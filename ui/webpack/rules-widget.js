const autoprefixer = require("autoprefixer");

const createRules = () => {
    return [
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
                {
                    loader: "style-loader",
                },
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
                {
                    loader: "sass-loader",
                    options: {
                        sassOptions: {
                            includePaths: ["src/widget/widgetcore/scss"],
                        },
                    },
                    exclude: /node_modules/,
                },
            ],
        },
        {
            test: /\.(jpg|png|gif|svg)$/,
            use: "url-loader",
        },
    ];
};

module.exports = {
    createRules,
};
