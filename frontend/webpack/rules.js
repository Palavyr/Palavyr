const path = require('path');

const TypeScriptLoaderRule = () => {
    return {
        test: /\.tsx?$/,
        use: [
            { loader: 'ts-loader', options: { transpileOnly: true } },
        ],
        exclude: /node_modules/
    }
}

const FileLoaderRule = () => {
    return {
        test: /\.(woff(2)?|ttf|eot|svg)(\?v=\d+\.\d+\.\d+)?$/,
        use: [
            {
                loader: 'file-loader',
                options: {
                    name: '[name].[ext]',
                    outputPath: 'fonts/'
                }
            }
        ]
    }
}

const SVGRule = () => {
    return {
        test: /\.svg$/,
        use: [
            { loader: 'svg-inline-loader' }
        ],
        exclude: path.resolve(__dirname, '/node_modules')
    }
}

const BabelLoaderRule = () => {
    return {
        test: /\.js$/,
        use: [
            { loader: 'babel-loader' }
        ],
        exclude: /node_modules/
    }
}

const StylesLoader = () => {

    return {
        test: /\.css$/i,
        use: [
            { loader: 'style-loader' },
            { loader: 'css-loader' },
            // { loader: 'css-to-mui-loader' },
            // { loader: "sass-loader" },
            // {
            //     loader: 'less-loader',
            //     options: {
            //         lessOptions: {
            //             strictMath: true,
            //         },
            //     },
            // }
        ],
        exclude: path.resolve(__dirname, '/node_modules')
    }
}

const URLLoaderRule = () => {
    return {
        test: /\.(png|jpg|gif)$/i,
        use: [
            {
                loader: 'url-loader',
                options: {
                    limit: 8192,
                },
            },
        ],
        exclude: path.resolve(__dirname, '/node_modules')
    }
}

const MUILoaderRule = () => {
    return {
        test: /\.css$/,
        use: ['css-to-mui-loader']
    }
}


module.exports = {
    TypeScriptLoaderRule,
    FileLoaderRule,
    SVGRule,
    BabelLoaderRule,
    StylesLoader,
    URLLoaderRule,
    MUILoaderRule
}