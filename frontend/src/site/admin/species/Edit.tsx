import * as React from "react"
import { Dispatch, SetStateAction, useState, useEffect } from "react"
import { useParams } from "react-router-dom"
import { getSpeciesById } from "@/tools/browser/species"
import { SpeciesModel } from "@/tools/definitions/species"
import Add from "@/site/admin/species/AddEdit"

const Edit: React.FunctionComponent = (): JSX.Element => {

	const [species, setSpecies]: [SpeciesModel, Dispatch<SetStateAction<SpeciesModel>>] = useState(undefined)
	const { speciesId } = useParams()

	const fetchSpecies = async () => {
		const result = await getSpeciesById(speciesId)
		if (result.wasSuccessful)
			setSpecies(result.result)
	}

	useEffect(() => {
		fetchSpecies()
	}, [])

	return (
		<>
			{species && (
				<Add species={species} />
			)}
		</>
	)
}

export default Edit