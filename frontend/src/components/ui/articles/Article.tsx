import * as React from "react"
import { PropsWithChildren } from "react"

import "./Article.scss"

const Article: React.FunctionComponent = ({ children }: PropsWithChildren<unknown>): JSX.Element => {
	return (
		<article className="article">
			{ children }
		</article>
	)
}

export default Article