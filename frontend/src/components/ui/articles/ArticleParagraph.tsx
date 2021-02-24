import * as React from "react"

type ArticleParagraphProps = {
	preamble?: boolean,
	children?: React.ReactNode
}

const ArticleParagraph: React.FunctionComponent<ArticleParagraphProps> = ({ preamble = false, children }: ArticleParagraphProps): JSX.Element => {

	const classes = `article__paragraph ${preamble ? "article__paragraph--preamble" : ""}`
	return (
		<p className={classes}>
			{ children }
		</p>
	)
}

export default ArticleParagraph