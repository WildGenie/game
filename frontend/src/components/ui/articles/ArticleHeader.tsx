import * as React from "react"
import { HeadingLevelProps } from "@/components/ui/articles/types"
import ArticleHeading from "@/components/ui/articles/ArticleHeading"

type ArticleHeaderProps = {
	children?: React.ReactNode
} & HeadingLevelProps

const ArticleHeader: React.FunctionComponent<ArticleHeaderProps> = ({ children, level = 1 }: ArticleHeaderProps): JSX.Element => {

	return (
		<header className="article__header">
			<ArticleHeading level={level}>
				{ children }
			</ArticleHeading>
		</header>
	)
}

export default ArticleHeader