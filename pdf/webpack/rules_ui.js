const path = require("path");
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const autoprefixer = require("autoprefixer");

const TypeScriptLoaderRule = () => {
    return {
        test: /\.tsx?$/,
        use: [{ loader: "ts-loader", options: { transpileOnly: true } }],
        exclude: /node_modules/,
    };
};

const NewFileLoaderRule = () => {
    return {
        test: /\.(png|jp(e*)g|gif)$/,
        use: [
            {
                loader: "file-loader",
                options: {
                    name: "images/[hash]-[name].[ext]",
                },
            },
        ],
    };
};

const SVGRule = () => {
    return {
        test: /\.svg$/,
        use: [{ loader: "svg-inline-loader" }],
        exclude: path.resolve(__dirname, "/node_modules"),
    };
};

const LiteLoadSVGs = () => {
    return {
        test: /\.svg$/,
        use: [
            {
                loader: "babel-loader",
            },
            {
                loader: "react-svg-loader",
                query: {
                    svgo: {
                        // pretty: true,
                        plugins: [{ removeStyleElement: false }],
                    },
                },
                options: {
                    jsx: true, // true outputs JSX tags
                },
            },
        ],
    };
};

const SVGRLoader = () => {
    return {
        test: /\.svg$/,
        use: ["@svgr/webpack"],
    };
};

const BabelLoaderRule = () => {
    return {
        test: /\.js$/,
        use: [{ loader: "babel-loader" }],
        exclude: /node_modules/,
    };
};

const StylesLoader = () => {
    return {
        test: /\.css$/i,
        use: [
            MiniCssExtractPlugin.loader,
            // 'style-loader',
            "css-loader",
        ],
        exclude: path.resolve(__dirname, "/node_modules"),
    };
};

const CSSModules = () => {
    return {
        test: /\.css$/,
        use: [
            "style-loader",
            {
                loader: "css-loader",
                options: {
                    importLoaders: 1,
                    modules: true,
                },
            },
        ],
    };
};

const URLLoaderRule = () => {
    return {
        test: /\.(png|jpg|gif)$/i,
        use: [
            {
                loader: "url-loader",
                options: {
                    limit: 8192,
                },
            },
        ],
        exclude: path.resolve(__dirname, "/node_modules"),
    };
};

const MUILoaderRule = () => {
    return {
        test: /\.css$/,
        use: ["css-to-mui-loader"],
    };
};

const CSSMinify = () => {
    return {
        test: /\.css$/i,
        use: [MiniCssExtractPlugin.loader, "css-loader"],
    };
};

const ScssLoaderRule = () => {
    return {
        test: /\.s[ac]ss$/i,
        use: [
            // Creates `style` nodes from JS strings
            "style-loader",
            // Translates CSS into CommonJS
            // "css-loader",
            // Compiles Sass to CSS
            // "sass-loader",
        ],
    };
};

const WidgetSassRule = () => {
    return {
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
            // {
            //     loader: "sass-loader",
            //     options: {
            //         sassOptions: {
            //             includePaths: ["src", "**/*"],
            //         },
            //     },
            // },
        ],
    };
};

module.exports = {
    NewFileLoaderRule,
    SVGRLoader,
    LiteLoadSVGs,
    CSSModules,
    CSSMinify,
    TypeScriptLoaderRule,
    SVGRule,
    BabelLoaderRule,
    StylesLoader,
    URLLoaderRule,
    MUILoaderRule,
    ScssLoaderRule,
    WidgetSassRule,
};
