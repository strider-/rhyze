import { getAlbums, Album } from "../services/api_service";
import { AuthProps } from "./private_route";
import { useEffect, useState } from "react";
import { Card } from "react-bootstrap";
import { ErrorResponse } from "../services/error";

const Albums = ({auth}: AuthProps) => {
    const [state, setState] = useState<ErrorResponse | Album[]>(null);

    useEffect(() =>{
        const fetch = async () => {
            const result = await getAlbums(auth, 0, 50);
            setState(result);
        };
        fetch();
    }, []);

    if(state == null) {
        return false;
    } else if(Array.isArray(state)){
        const albums = state as Album[];
        return <div className="d-flex flex-row card-columns">
            {albums.map(a => <AlbumArt key={a.id} {...a} />)}
        </div>;
    } else {
        return <h3 className='text-danger'>{state.error}</h3>
    }
}

const AlbumArt = ({imageUrl, name, artist}) => (<div className="p-2">
    <Card style={{maxWidth: '184px'}}>
        <Card.Header style={{padding: 0}} className="text-center">
            <small>{name}</small>
        </Card.Header>
        <Card.Img src={imageUrl} style={{ height: '184px', width: '184px'}} />
        <Card.Footer style={{padding: 0}} className="text-center">
            <small className='text-muted text-center'>{artist}</small>
        </Card.Footer>
    </Card>
</div>);

export default Albums;