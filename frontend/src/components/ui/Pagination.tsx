import * as React from "react"
import { Dispatch, SetStateAction } from "react"

import "./Pagination.scss"
import SelectField from "@/components/ui/forms/fields/SelectField"
import { FormSelect } from "@/tools/definitions/forms"

type PaginationProps = {
	resultsPerPage: number,
	setResultsPerPage: Dispatch<SetStateAction<number>>,
	totalEntries: number,
	page: number,
	setPage: Dispatch<SetStateAction<number>>
}

const Pagination: React.FunctionComponent<PaginationProps> = ({
	resultsPerPage,
	setResultsPerPage,
	totalEntries,
	page,
	setPage
}: PaginationProps): JSX.Element => {

	const totalPages = Math.ceil(totalEntries / resultsPerPage)

	const resultsOptions: FormSelect[] = [
		{
			text: 10,
			value: 10
		},
		{
			text: 25,
			value: 25
		},
		{
			text: 50,
			value: 50
		}
	]

	return (
		<section className="pagination">

			<SelectField
				options={resultsOptions}
				labelText="Results per page"
				value={resultsPerPage}
				setValue={setResultsPerPage}
			/>

			<nav className="pagination__nav">
				<button
					className="pagination__nav-button"
					title="Go to the first page"
					onClick={() => setPage(1)}
				>
					<i className="fal fa-arrow-to-left"/>
				</button>

				{[-2, -1, 0, 1, 2].map(relativePage => {
					const targetPage = page + relativePage
					if (targetPage < 1 || targetPage > totalPages)
						return null

					return (
						<button
							key={relativePage}
							className="pagination__nav-button"
							title={`Go to page ${targetPage}`}
							onClick={() => setPage(targetPage)}
						>
							{targetPage}
						</button>
					)
				})}

				<button
					className="pagination__nav-button"
					title="Go to the last page"
					onClick={() => setPage(totalPages)}
				>
					<i className="fal fa-arrow-to-right"/>
				</button>
			</nav>
		</section>
	)
}

export default Pagination