import { useRouter } from 'next/router'
import { AuthProps, privateRoute } from '../../components/private_route'
import { useState, useEffect } from 'react'
import { ErrorResponse } from '../../services/error'
import { Track, getAlbum } from '../../services/api_service'
import Loading from '../../components/loading'

const Album = ({auth}: AuthProps) => {
  const router = useRouter()
  const { aid } = router.query
  const [state, setState] = useState<ErrorResponse | Track[]>(null)

  useEffect(() =>{
      const fetch = async () => {
          const result = await getAlbum(auth, aid as string);
          setState(result);
      }
      fetch()
  });

  if(state == null){
      return <Loading />;
  } else if(Array.isArray(state)){
      return <ul>
          {state.map(t => <li>{t.trackNo} - {t.title} [{asTimeString(t.duration)}]</li>)}
      </ul>;
  } else {
    return <h3 className='text-danger'>{state.error}</h3>
  }
}

const asTimeString = (seconds: number): string => {
    var date = new Date(0);
    date.setSeconds(Math.ceil(seconds));
    return date.toISOString().substr(14, 5);
};

export default privateRoute(Album);