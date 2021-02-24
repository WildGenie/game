import * as React from "react"
import { Dispatch, SetStateAction, useEffect, useState } from "react"
import { NavLink, useRouteMatch } from "react-router-dom"

import Table from "@/components/ui/tables/Table"
import TableHead from "@/components/ui/tables/TableHead"
import TableRow from "@/components/ui/tables/TableRow"
import HeaderCell from "@/components/ui/tables/HeaderCell"
import TableBody from "@/components/ui/tables/TableBody"
import BodyCell from "@/components/ui/tables/BodyCell"
import { SpeciesModel } from "@/tools/definitions/species"
import { getSpecies } from "@/tools/browser/species"
import { getDocumentTitle } from "@/tools/utils"

const Home: React.FunctionComponent = (): JSX.Element => {

	const { url } = useRouteMatch()

	const [speciesList, setSpeciesList]: [SpeciesModel[], Dispatch<SetStateAction<SpeciesModel[]>>] = useState([])

	const fetchSpecies = async () => {
		const response = await getSpecies()
		if (response.wasSuccessful)
			setSpeciesList(response.result)
	}

	useEffect(() => {
		fetchSpecies()
	}, [])

	document.title = getDocumentTitle("Species")

	return (
		<Table>
			<TableHead>
				<TableRow>
					<HeaderCell width="8rem">
						Species
					</HeaderCell>
					<HeaderCell width="auto">
						Description
					</HeaderCell>
					<HeaderCell width="4rem">
						Actions
					</HeaderCell>
				</TableRow>
			</TableHead>

			<TableBody>
				{speciesList.map((species, i) => (
					<TableRow key={i}>
						<BodyCell>{species.name}</BodyCell>
						<BodyCell>{species.description}</BodyCell>
						<BodyCell>
							<NavLink
								to={`${url}/${species.id}`}
								title={`Edit ${species.name}`}
							>
								<i className="fal fa-edit"/>
							</NavLink>
						</BodyCell>
					</TableRow>
				))}
			</TableBody>
		</Table>
	)
}

export default Home