const path = require("path")
const webpack = require("webpack")
const HtmlWebpackPlugin = require("html-webpack-plugin")
const CopyWebpackPlugin = require("copy-webpack-plugin")
const MiniCssExtractPlugin = require("mini-css-extract-plugin")
const {CleanWebpackPlugin} = require("clean-webpack-plugin")

module.exports = ({development}) => {
	const devmode = !!development
	return {
		target: "web",
		entry: {
			app: "./src/index.tsx"
		},
		output: {
			path: path.join(__dirname, "build"),
			filename: devmode
				? "js/[name].js"
				: "js/[name].[hash].js",
			publicPath: "/"
		},
		devServer: {
			allowedHosts: ["*"],
			hot: true,
			watchOptions: {
				poll: true
			},
			historyApiFallback: true,
			inline: true,
			publicPath: "/",
			proxy: {
				"/api": {
					target: "http://localhost:5000",
					pathRewrite: {
						"^/api": ""
					}
				}
			}
		},
		plugins: [
			new webpack.HotModuleReplacementPlugin(),
			new HtmlWebpackPlugin({
				template: "src/index.html",
				filename: "index.html",
				inject: false
			}),
			new CopyWebpackPlugin(
				{
					patterns: [
						{
							from: "src/assets",
							to: ""
						}
					]
				}),
			new MiniCssExtractPlugin(
				{
					filename: devmode
						? "css/[name].css"
						: "css/[name].[hash].css",
					chunkFilename: devmode
						? "css/[id].css"
						: "css/[id].[hash].css",
					ignoreOrder: false
				}),
			new CleanWebpackPlugin()
		],
		module: {
			rules: [
				{
					test: /\.tsx?$/,
					exclude: /node_modules/,
					use: [
						{
							loader: "babel-loader"
						},
						{
							loader: "ts-loader"
						}
					]
				},
				{
					test: /\.(sc|sa|c)ss$/,
					use: [
						{
							loader: MiniCssExtractPlugin.loader,
							options: {
								hmr: devmode
							}
						},
						"css-loader",
						{
							loader: "postcss-loader",
							options: {
								plugins: [
									require("autoprefixer")
								]
							}
						},
						{
							loader: "sass-loader",
							options: {
								implementation: require("sass"),
								sassOptions: {}
							}
						}
					]
				},
				{
					test: /\.(png|svg|jpg|jpeg)$/i,
					type: "asset/resource"
				}
			]
		},
		resolve: {
			extensions: [
				".ts",
				".js",
				".tsx",
				".jsx"
			],
			alias: {
				"@": path.join(__dirname, "src")
			}
		},
		devtool: devmode
			? "source-map"
			: false,
		mode: devmode
			? "development"
			: "production"
	}
}