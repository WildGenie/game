import * as React from "react"
import { PropsWithChildren } from "react"
import { HeadingLevelProps } from "@/components/ui/articles/types"

type ArticleHeadingProps = {
	children?: React.ReactNode
} & HeadingLevelProps

const ArticleHeading: React.FunctionComponent<ArticleHeadingProps> = ({ children, level = 1 }: ArticleHeadingProps): JSX.Element => {

	const Heading = `h${level}` as keyof JSX.IntrinsicElements

	return (
		<Heading className={`article__heading article__heading--${level}`}>
			{ children }
		</Heading>
	)
}

export default ArticleHeading