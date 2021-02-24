import * as React from "react"
import ArticleHeader from "@/components/ui/articles/ArticleHeader"

type ArticleSectionProps = {
	preamble?: boolean,
	headerText?: string,
	headingLevel?: number,
	children?: React.ReactNode
}

const ArticleSection: React.FunctionComponent<ArticleSectionProps> = ({
	preamble = false,
	headerText,
	headingLevel = 2,
	children
}: ArticleSectionProps): JSX.Element => {

	const sectionClasses = `article__section ${preamble ? "article__section--preamble" : ""}`

	return (
		<section className={sectionClasses}>
			{headerText && (
				<ArticleHeader level={headingLevel}>
					{ headerText }
				</ArticleHeader>
			)}

			{ children }
		</section>
	)
}

export default ArticleSection